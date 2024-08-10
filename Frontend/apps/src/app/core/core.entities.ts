
export type HomeVm = {
    flightManagementSummary: FlightManagementSummary;
    flightShoppingSummary: FlightShoppingSummary;
};

export type FlightManagementSummary = {
    flightCount: number;
    departuresCount: number;
    destinationsCount: number;
};

export type FlightShoppingSummary = {
    maxBasePrice: number;
    minBasePrice: number;
    averagePrice: number;
};

export const initialHomeVm: HomeVm = {
    flightManagementSummary: {
        flightCount: 0,
        departuresCount: 0,
        destinationsCount: 0
    },
    flightShoppingSummary: {
        maxBasePrice: 0,
        minBasePrice: 0,
        averagePrice: 0
    }
}
