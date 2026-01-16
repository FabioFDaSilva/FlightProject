// Flight segment
export interface Segment {
    flightNo: string;
    flightId: string;
    departCode: string;
    departDate: string;
    departTime: string;
    departTerminal: string;
    arrivalCode: string;
    arrivalDate: string;
    arrivalTime: string;
    arrivalTerminal: string;
    bookingClass: string;
    flightClass: string;
    journey: string;
  }
  
  // Flight itself
  export interface Flight {
    id: string;
    carrier: string;
    depAir: string;
    destAir: string;
    inArrivalDate: string;
    inArrivalTime: string;
    inArriveCode: string;
    inBookingClass: string;
    inCarrierCode: string;
    inDepartCode: string;
    inDepartDate: string;
    inDepartTime: string;
    inFlightClass: string;
    inFlightNo: string;
    oneWay: string;
    originalCurrency: string;
    originalPrice: string;
    outArrivalDate: string;
    outArrivalTime: string;
    outBookingClass: string;
    outCarrierCode: string;
    outDepartDate: string;
    outDepartTime: string;
    outFlightClass: string;
    outFlightNo: string;
    reservation: string;
    segments: Segment[];
  }

  export interface AirportCount {
    airports: {
      name: string;   // IATA code
      count: number;  // number of flights
    }[];
  }