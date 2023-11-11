
export type FlightConnection = {
    from: string; 
    to: string;
    icaoFrom: string; 
    icaoTo: string;
};

export type FlightTimes = {
    takeOff: string; 
    landing: string;
};

export type FlightOperator = {
    //name: string; 
    shortName: string;
    seatCount: number;
};

export type FlightPrice = {
    basePrice: number; 
    seatReservationSurcharge: number;
};

export type FlightEditViewModel = { 
    connection: FlightConnection; 
    times: FlightTimes;
    operator: FlightOperator;
    price: FlightPrice;
};
