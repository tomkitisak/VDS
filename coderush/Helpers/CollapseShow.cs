using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace vds.Helpers
{
    [HtmlTargetElement("div",Attributes = ControllersAttributeName)]
    [HtmlTargetElement("div" ,Attributes = ActionsAttributeName)]
    [HtmlTargetElement("div",Attributes = RouteAttributeName)]
    [HtmlTargetElement("div",Attributes = ClassAttributeName)]
    [HtmlTargetElement("div", Attributes = "collapse-show")]


    public class CollapseShow : TagHelper
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
        public string Class { get; set; } = "show";



        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("collapse-show")]
        public string IsShow { get; set; }


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

            if (acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) && IsShow == "true")
            {
                //Get the current value of the class attribute
                var currentClassValue = "";
                if (output.Attributes.ContainsName("class"))
                {
                    currentClassValue = output.Attributes["class"].Value.ToString();
                    output.Attributes.Remove(output.Attributes["class"]);
                }

                //Add a new class attribute with the previous values
                output.Attributes.Add("class", "collapse show " + currentClassValue);

            }
            else
            {
               
                SetAttribute(output, "class", "collapse");
            }

          // base.Process(context, output);
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
