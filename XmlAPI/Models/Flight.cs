using System.Xml.Serialization;
namespace XmlAPI.Models
{
    public class Flight
    {
        [XmlAttribute("carrier")]
        public string Carrier { get; set; } = string.Empty;
        [XmlAttribute("depair")]
        public string DepAir { get; set; } = string.Empty;
        [XmlAttribute("destair")]
        public string DestAir { get; set; } = string.Empty;
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;
         [XmlAttribute("inarrivaldate")]
        public string InArrivalDate { get; set; } = string.Empty;
        [XmlAttribute("inarrivaltime")]
        public string InArrivalTime { get; set; } = string.Empty;
        [XmlAttribute("inarrivecode")]
        public string InArriveCode { get; set; } = string.Empty;
        [XmlAttribute("inbookingclass")]
        public string InBookingClass { get; set; } = string.Empty;
        [XmlAttribute("incarriercode")]
        public string InCarrierCode { get; set; } = string.Empty;
        [XmlAttribute("outarrivecode")]
        public string OutArrivalDate { get; set; } = string.Empty;
        [XmlAttribute("indepartcode")]
        public string InDepartCode { get; set; } = string.Empty;
        [XmlAttribute("indepartdate")]
        public string InDepartDate { get; set; } = string.Empty;
        [XmlAttribute("indeparttime")]
        public string InDepartTime { get; set; } = string.Empty;
        [XmlAttribute("inflightclass")]
        public string InFlightClass { get; set; } = string.Empty;
        [XmlAttribute("inflightno")]
        public string InFlightNo { get; set; } = string.Empty;
        [XmlAttribute("oneway")]
        public string OneWay { get; set; } = string.Empty;
        [XmlAttribute("originalcurrency")]
        public string OriginalCurrency { get; set; } = string.Empty;
        [XmlAttribute("originalprice")]
        public string OriginalPrice { get; set; } = string.Empty;
        [XmlAttribute("outarrivaltime")]
        public string OutArrivalTime { get; set; } = string.Empty;
        [XmlAttribute("outbookingclass")]
        public string OutBookingClass { get; set; } = string.Empty;
        [XmlAttribute("outcarriercode")]
        public string OutCarrierCode { get; set; } = string.Empty;
        [XmlAttribute("outdepartdate")]
        public string OutDepartDate { get; set; } = string.Empty;
        [XmlAttribute("outdeparttime")]
        public string OutDepartTime { get; set; } = string.Empty;
        [XmlAttribute("outflightclass")]
        public string OutFlightClass { get; set; } = string.Empty;
        [XmlAttribute("outflightno")]
        public string OutFlightNo { get; set; } = string.Empty;
        [XmlAttribute("reservation")]
        public string Reservation { get; set; } = string.Empty;

        [XmlArray("segments")]            // The parent XML element <segments>
        [XmlArrayItem("segment")]         // Each child <segment> element
        public List<Segment> Segments { get; set; } = new();

    }

}