import { Http } from '@angular/http';
import { Response } from '@angular/http/src/static_response';


export class ServiceResponse<T> {

    Value: T;
    StatusCode: Number;
    StatusMessage: String;
    Ok: Boolean;

    static async CreateServiceResponse<T>(response: Response): Promise<ServiceResponse<T>> {
        const res = new ServiceResponse<T>();
        res.Value = (await response.json()) as T;
        res.StatusCode = response.status;
        res.StatusMessage = response.statusText;
        res.Ok = response.ok;
        return res;
    }
}
