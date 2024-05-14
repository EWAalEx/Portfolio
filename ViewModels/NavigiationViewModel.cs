using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AlexPortfolio.ViewModels
{
    public class NavigationViewModel
    {
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public MediaWithCrops? BackgroundMedia { get; set; }
        public string? BackgroundColour { get; set; }
        
        //for navigation links
        public List<Link> Links { get; set; }
        public List<Link> ChildLinks1 { get; set; }
        public List<Link> ChildLinks2 { get; set; }
        public List<Link> ChildLinks3 { get; set; }
        public List<Link> ChildLinks4 { get; set; }
        public List<Link> ChildLinks5 { get; set; }
        public List<bool> HeadingChildrens { get; set; }
        public string? Email { get; set; }
        public string? SecondarySiteColour { get; set; }
        public string? MainTextColour { get; set; }
    }
}
