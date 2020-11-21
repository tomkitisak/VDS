using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace vds.Helpers
{

    [HtmlTargetElement("li", Attributes = ControllersAttributeName)]
    [HtmlTargetElement("li", Attributes = ActionsAttributeName)]
    [HtmlTargetElement("li", Attributes = RouteAttributeName)]
    [HtmlTargetElement("li", Attributes = ClassAttributeName)]


    public class ActiveLinkTagHelper : TagHelper
    {
        private const string ActionsAttributeName = "asp-active-actions";
        private const string ControllersAttributeName = "asp-active-controllers";
        private const string ClassAttributeName = "asp-active-class";
        private const string RouteAttributeName = "asp-active-route";
   
        [HtmlAttributeName(ControllersAttributeName)]
        public string Controllers { get; set; } = "";

        [HtmlAttributeName(ActionsAttributeName)]
        public string Actions { get; set; } = "";

        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; } = "";

        [HtmlAttributeName(ClassAttributeName)]
        public string Class { get; set; } = "active";

   

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }



        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            RouteValueDictionary routeValues = ViewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (Actions.Length <= 0)
                Actions = currentAction;

            if (Controllers.Length <= 0)
                Controllers = currentController;
         
            string[] acceptedActions = Actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = Controllers.Trim().Split(',').Distinct().ToArray();
        
            if (acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController))
            {
                SetAttribute(output, "class", "nav-item " + Class);
            }
            else
            {
                SetAttribute(output, "class", "nav-item");
            }

            base.Process(context, output);
        }

        private void SetAttribute(TagHelperOutput output, string attributeName, string value, bool merge = true)
        {
            var v = value;


            if (output.Attributes.TryGetAttribute(attributeName, out TagHelperAttribute attribute))
            {
                if (merge)
                {
                    v = $"{attribute.Value} {value}";
                }
            }
            output.Attributes.SetAttribute(attributeName, v);
        }
    }
}
