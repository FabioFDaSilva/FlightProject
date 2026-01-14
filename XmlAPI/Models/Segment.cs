using System;
using System.Xml.Serialization;

public class Segment
{
    [XmlAttribute("arrcode")]
    public string ArrivalCode { get; set; } = string.Empty;

    [XmlAttribute("arrdate")]
    public string ArrivalDate { get; set; } = string.Empty;

    [XmlAttribute("arrtime")]
    public string ArrivalTime { get; set; } = string.Empty;

    [XmlAttribute("arrterminal")]
    public string ArrivalTerminal { get; set; } = string.Empty;

    [XmlAttribute("bookingclass")]
    public string BookingClass { get; set; } = string.Empty;

    [XmlAttribute("class")]
    public string FlightClass { get; set; } = string.Empty;

    [XmlAttribute("depcode")]
    public string DepartCode { get; set; } = string.Empty;

    [XmlAttribute("depdate")]
    public string DepartDate { get; set; } = string.Empty;

    [XmlAttribute("deptime")]
    public string DepartTime { get; set; } = string.Empty;

    [XmlAttribute("depterminal")]
    public string DepartTerminal { get; set; } = string.Empty;

    [XmlAttribute("flightid")]
    public string FlightId { get; set; } = string.Empty;

    [XmlAttribute("flightno")]
    public string FlightNo { get; set; } = string.Empty;

    [XmlAttribute("journey")]
    public string Journey { get; set; } = string.Empty;
}