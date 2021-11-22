using Microsoft.AspNetCore.Mvc.Razor;

namespace Autowriter
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            yield return "/SharedViews/{0}.cshtml";
            yield return "/Features/{1}/{0}.cshtml";
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
