import { AppConsts } from '@shared/AppConsts';
import { UtilsService, TokenService } from '@yoyo/abp';
import { HttpHeaders, HttpClient, HttpRequest, HttpEventType, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenHelper } from './TokenHelper';
import { UploadXHRArgs } from 'ng-zorro-antd';

export class FileUploadHelper {

    /**
     * 上传文件
     * @param httpClient httpClient
     * @param uploadUrl 上传地址
     * @param formData 表单数据
     * @param successedCallBack 上传成功的回调
     * @param failedCallBack 上传失败的回调
     * @param finallyCallBack 上传(成功/失败)的回调
     */
    static uploadFile(
        httpClient: HttpClient,
        uploadUrl: string,
        formData: FormData,
        successedCallBack?: (result) => void,
        failedCallBack?: (error) => void,
        finallyCallBack?: () => void
    ): void {
        if (!finallyCallBack) {
            finallyCallBack = () => { };
        }

        let tokenHeader = TokenHelper.createRequestHeaders();
        const req = new HttpRequest('POST', uploadUrl, formData, {
            headers: tokenHeader,
            reportProgress: true,
            withCredentials: true
        });
        httpClient.request(req)
            .finally(finallyCallBack)
            .subscribe((event: HttpEvent<{}>) => {
                if (event instanceof HttpResponse) {
                    // 上传成功回调
                    if (successedCallBack) {
                        successedCallBack((<any>event.body).result);
                    }
                }
            }, (err) => {
                // 失败回调
                if (failedCallBack) {
                    failedCallBack(err);
                }
            });
    }


}