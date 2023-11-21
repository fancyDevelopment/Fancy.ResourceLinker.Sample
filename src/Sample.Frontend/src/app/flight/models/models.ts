import { Resource } from "fancy-hateoas-client";

export type FlightConnection = Resource & {
    from: string; 
    to: string;
    icaoFrom: string; 
    icaoTo: string;
};

export type FlightTimes = Resource & {
    takeOff: string; 
    landing: string;
};

export type FlightOperator = Resource & {
    //name: string; 
    shortName: string;
    seatCount: number;
};

export type FlightPrice = Resource & {
    basePrice: number; 
    seatReservationSurcharge: number;
};

export type FlightEditViewModel = Resource & { 
    connection: FlightConnection; 
    times: FlightTimes;
    operator: FlightOperator;
    price: FlightPrice;
};
