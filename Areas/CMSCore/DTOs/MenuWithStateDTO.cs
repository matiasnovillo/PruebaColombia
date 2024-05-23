using PruebaColombia.Areas.CMSCore.Entities;

namespace PruebaColombia.Areas.CMSCore.DTOs
{
    public class MenuWithStateDTO : Menu
    {
        public bool IsSelected { get; set; }
    }
}
