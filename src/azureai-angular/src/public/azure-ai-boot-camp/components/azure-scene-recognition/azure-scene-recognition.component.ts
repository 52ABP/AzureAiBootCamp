import { Component, OnInit, Injector, Input, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AzureServiceProxy, ImgSceneRecognitionDto, FaceDescriptionGender, ImgSceneRecognitionInput } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-azure-scene-recognition',
  templateUrl: './azure-scene-recognition.component.html',
  styleUrls: [
    './azure-scene-recognition.component.less'
  ]
})
export class AzureSceneRecognitionComponent extends AppComponentBase
  implements OnInit {

  @ViewChild("imgContainer") imgContainer: ElementRef;
  @ViewChild("img") img: ElementRef;

  isLoding: boolean;

  requestParms: ImgSceneRecognitionInput = new ImgSceneRecognitionInput();
  entityDto: ImgSceneRecognitionDto;

  faces: any[] = [];

  result: string;

  constructor(
    injector: Injector,
    private renderer: Renderer2,
    private _azureService: AzureServiceProxy,
  ) {
    super(injector);

  }
  ngOnInit() {
    this.requestParms.imgUrl = "http://n.sinaimg.cn/sinacn15/329/w640h489/20181114/fa10-hnvukfe8865615.jpg";
  }

  /**
  * 
  */
  analyze() {

    this.isLoding = true;
    this.entityDto = null;
    this.result = '';
    this.faceRectangleClear();

    this._azureService.imgSceneRecognition(this.requestParms)
      .finally(() => {
        this.isLoding = false;
      })
      .subscribe((result: ImgSceneRecognitionDto) => {
        this.entityDto = result;

        this.faceRectangleCreate(this.entityDto);

        this.result += "图片场景说明:\n";
        result.captions.forEach((item) => {
          this.result += item + "\n";
        })

        this.result += "\n\n图片标签:\n";
        result.imgTags.forEach((item) => {
          this.result += "标签名称: " + item.tagName + "  匹配度: " + item.percentageStr + "\n";
        });


        this.result += "\n\n人脸信息:\n";

        this.result += JSON.stringify(result.faces);




      })
  }

  /**
   * 移除面部勾勒框
   */
  faceRectangleClear(): void {
    // 移除面部勾勒框
    this.faces.forEach((item) => {
      this.renderer.removeChild(this.imgContainer.nativeElement, item);
    });
    this.faces = [];
  }

  /**
   * 创建面部勾勒框
   * @param input 
   */
  faceRectangleCreate(input: ImgSceneRecognitionDto) {

    // 页面图片的宽高
    let pageImgWidth: number = this.img.nativeElement.offsetWidth;
    let pageImgHeight: number = this.img.nativeElement.offsetHeight;

    // 图片真实宽高
    let imgWidth: number = input.width;
    let imgHeight: number = input.height;

    // 页面和图片的宽高比例补偿
    let wp = pageImgWidth / imgWidth;
    let hp = pageImgHeight / imgHeight;



    // 添加面部勾勒框
    if (input.faces && input.faces.length > 0) {

      input.faces.forEach((item) => {
        let color = "#000";// 默认黑色边框

        switch (item.gender) {
          case FaceDescriptionGender.Male:
            color = "#9cdcfe";
            break;
          case FaceDescriptionGender.Female:
            color = "#ce9178";
            break;
        }

        // 组织dom
        var faceDiv = this.renderer.createElement("div");
        this.renderer.setStyle(faceDiv, "width", (item.faceRectangle.width * wp) + "px");
        this.renderer.setStyle(faceDiv, "height", (item.faceRectangle.height * hp) + "px");
        this.renderer.setStyle(faceDiv, "left", (item.faceRectangle.left * wp) + "px");
        this.renderer.setStyle(faceDiv, "top", (item.faceRectangle.top * hp) + "px");
        this.renderer.setStyle(faceDiv, "position", "absolute");
        this.renderer.setStyle(faceDiv, "border", "solid 3px " + color);
        // 添加到记录数组
        this.faces.push(faceDiv);
        // 添加到dom容器
        this.renderer.appendChild(this.imgContainer.nativeElement, faceDiv);

      });
    }
  }

}
