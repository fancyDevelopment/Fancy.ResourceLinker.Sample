import { AxiosRequestManager, SignalRSocketManager } from ".";
import { HateoasClient } from "./hateoas-client";

export function createDefaultHateoasClient() {
    return new HateoasClient(new AxiosRequestManager(), new SignalRSocketManager());
}