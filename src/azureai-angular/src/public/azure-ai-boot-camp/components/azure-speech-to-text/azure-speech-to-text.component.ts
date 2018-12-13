import { Component, OnInit, Injector, Renderer2, ViewChild, ElementRef, Input, Output } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AzureServiceProxy, ImgSceneRecognitionDto, FaceDescriptionGender, TextToSpeechInput } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpHeaders, HttpClient, HttpRequest, HttpEventType, HttpEvent, HttpResponse } from '@angular/common/http';
import { TokenHelper } from '@shared/helpers/TokenHelper';
import { FileDownloadHelper } from '@shared/helpers/FileDownloadHelper';
import { EventEmitter } from 'events';
import { UploadXHRArgs, UploadFile } from 'ng-zorro-antd';
import { FileUploadHelper } from '@shared/helpers/FileUploadHelper';

@Component({
  selector: 'app-azure-speech-to-text',
  templateUrl: './azure-speech-to-text.component.html',
  styleUrls: [
    './azure-speech-to-text.component.less'
  ]
})
export class AzureSpeechToTextComponent extends AppComponentBase
  implements OnInit {


  isLoding: boolean;


  // 语言
  _langs: any[];
  @Input()
  set langs(value: any[]) {
    this._langs = value;
    if (value && value.length > 0) {
      this.selectedLang = "zh-CN";
    }
  }

  // 选中的语言
  selectedLang: string;

  // 上传的api接口
  uploadUrl: string;

  // 分析语音的结果
  resultText: string;

  // 上传文件列表
  fileList: UploadFile[] = [];

  constructor(
    injector: Injector,
    private httpClient: HttpClient,
    private _azureService: AzureServiceProxy,
  ) {
    super(injector);

  }
  ngOnInit() {
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + "/api/Azure/SpeechToText";
  }


  // 上传之前
  beforeUpload = (file: UploadFile): boolean => {
    this.fileList.length
    this.fileList = [];
    this.fileList.push(file);
    return false;
  }


  /**
  * 
  */
  analyze() {
    this.isLoding = true;
    this.resultText = '';
    const formData = new FormData();
    formData.append('file', this.fileList[0] as any);
    formData.append('lang', this.selectedLang);


    FileUploadHelper.uploadFile(this.httpClient, this.uploadUrl, formData,
      (result) => {// 成功回调
        this.resultText = result;
      },
      (error) => {// 失败回调
        this.message.error(error);
      },
      () => {// 结束回调
        this.isLoding = false;
      });
  }

  // 启用上传按钮
  enableUpload(): boolean {
    return this.fileList.length > 0;
  }

  // 删除文件
  removeFile = (file: UploadFile): boolean => {
    return !this.isLoding;
  }
}
