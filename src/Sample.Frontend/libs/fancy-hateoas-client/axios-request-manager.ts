import axios, { AxiosResponse } from "axios";
import { SecurityTokenProvider } from ".";
import { RequestManager } from "./request-manager";

export class AxiosRequestManager extends RequestManager {

    constructor(private _tokenProvider?: SecurityTokenProvider, private _additionalHeaders?: {[key: string]: string}, private _authorizationScheme?: string) {
        super();
    }

    protected async request(method: 'GET' | 'PUT' | 'POST' | 'DELETE', url: string, body?: any): Promise<any> {

        let headers: any = {};

        if(this._tokenProvider) {
            if(!this._authorizationScheme) {
                this._authorizationScheme = 'Bearer';
            }
            const accessToken = await this._tokenProvider.retrieveCurrentToken();
            headers['Authorization'] = this._authorizationScheme + ' ' + accessToken;
        }

        if(this._additionalHeaders) {
            for(let key in this._additionalHeaders) {
                headers[key] = this._additionalHeaders[key];
            }
        }

        let response: AxiosResponse<any>;

        switch(method) {
            case 'GET': 
                headers['Accept'] = 'application/json';
                response = await axios.get(url, { headers });
                break;
            case 'PUT':
                headers['Content-Type'] = 'application/json';
                response = await axios.put(url, body, { headers });
                break;
            case 'POST':
                headers['Content-Type'] = 'application/json';
                response = await axios.post(url, body, { headers });
                break;
            case 'DELETE':
                response = await axios.delete(url, { headers });
                break;
        }

        if(response.status === 200 || response.status === 201 || response.status === 203) {
            // On return codes 'OK', 'Created' and 'Non-Authoritative Information' return the response data
            return response.data;
        } else if(response.status === 202 || response.status === 204) {
            // On 'Accepted' and 'No Content' no contnet is retured
            return null;
        }

        throw {
            message: 'Unknown HTTP HATEOAS response',
            status: response.status,
            data: response.data
        }
    }
}