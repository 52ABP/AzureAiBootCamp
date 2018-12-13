import { AppConsts } from '@shared/AppConsts';
import { UtilsService, TokenService } from '@yoyo/abp';
import { HttpHeaders } from '@angular/common/http';

export class FileDownloadHelper {

    /**
     * 将blob响应保存为文件
     * @param response httpClient的响应
     * @param fileNameWithEx 文件名.文件扩展名
     */
    static responseDownloadFile(response: any, fileNameWithEx: string): void {
        let blob = new Blob([response.body], { type: response.body.type });
        let link = document.createElement("a");
        link.setAttribute("href", window.URL.createObjectURL(blob));
        link.setAttribute("download", fileNameWithEx);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }


}