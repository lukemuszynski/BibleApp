import { Http } from '@angular/http';
import { Response } from '@angular/http/src/static_response';

export class ServiceResponse<T> {

    Value: T;
    StatusCode: Number;
    StatusMessage: String;
    Ok: Boolean;

    static async CreateServiceResponse<T>(responsePromise: Promise<Response>): Promise<ServiceResponse<T>> {
        const res = new ServiceResponse<T>();
        let response;
        try {
            response = await responsePromise;
            res.Value = (await response.json()) as T;
            res.StatusCode = response.status;
            res.StatusMessage = response.statusText;
            res.Ok = response.ok;
        } catch (ex) {
            res.Value = ex;
            res.StatusCode = ex.status;
            res.StatusMessage = ex._body;
            res.Ok = ex.ok;
        }
        return res;
    }
}
