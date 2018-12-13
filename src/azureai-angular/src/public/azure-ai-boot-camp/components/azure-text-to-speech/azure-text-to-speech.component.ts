import { Component, OnInit, Injector, Renderer2, ViewChild, ElementRef, Input } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AzureServiceProxy, ImgSceneRecognitionDto, FaceDescriptionGender, TextToSpeechInput } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { TokenHelper } from '@shared/helpers/TokenHelper';
import { FileDownloadHelper } from '@shared/helpers/FileDownloadHelper';

@Component({
  selector: 'app-azure-text-to-speech',
  templateUrl: './azure-text-to-speech.component.html',
  styleUrls: [
    './azure-text-to-speech.component.less'
  ]
})
export class AzureTextToSpeechComponent extends AppComponentBase
  implements OnInit {
  @ViewChild("imgContainer")
  imgContainer: ElementRef;

  isLoding: boolean;

  // OCR识别使用的属性
  requestParms: TextToSpeechInput = new TextToSpeechInput();

  // 语言
  _langs: any[];
  @Input()
  set langs(value: any[]) {
    this._langs = value;
    if (value && value.length > 0) {
      // this.requestParms.lang = value[0].value;

      this.requestParms.lang = "zh-CN";
      this.langChanged(this.requestParms.lang);
    }
  }
  // 声音
  voices: any[] = [];


  constructor(
    injector: Injector,
    private httpClient: HttpClient,
    private renderer: Renderer2,
    private _azureService: AzureServiceProxy,
  ) {
    super(injector);

  }
  ngOnInit() {

  }

  /**
  * 
  */
  analyze() {
    this.isLoding = true;
    let baseUrl = AppConsts.remoteServiceBaseUrl;
    let url_ = baseUrl + "/api/Azure/TextToSpeech";

    const content_ = JSON.stringify(this.requestParms);
    let tokenHeader = TokenHelper.createRequestHeaders();
    tokenHeader = tokenHeader.set("Content-Type", "application/json");
    debugger
    let options_: any = {
      body: content_,
      observe: "response",
      responseType: "blob",
      headers: tokenHeader
    };


    this.httpClient.request("post", url_, options_)
      .finally(() => {
        this.isLoding = false;
      })
      .subscribe((response) => {
        // 将响应下载到文件
        let timestamp = (new Date()).valueOf();
        FileDownloadHelper.responseDownloadFile(response, timestamp + ".wav");
      });
  }


  /**
   * 语言发生修改
   * @param lang 语言编码
   */
  langChanged(lang: string) {
    if (lang) {
      var tmpLang = this._langs.find(item => item.value === lang);
      this.voices = tmpLang.children;
      this.requestParms.voice = this.voices[0].value;
    } else {
      this.requestParms.voice = null;
    }

  }

}
