namespace TheVilleSkill.Models.Eventful.Response
{
    public class EventImage : Image
    {
        public Image Small { get; set; }

        public Image Medium { get; set; }

        public Image Thumb { get; set; }

        public string Caption { get; set; }
    }
}
