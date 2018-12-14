import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: [
    './layout.component.less'
  ]
})
export class LayoutComponent extends AppComponentBase
  implements OnInit {

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }
  ngOnInit() {

  }


  showSourceCode() {
    let date = new Date();
    let year = date.getFullYear();
    let month = date.getMonth() + 1;
    let day = date.getDate()
    let hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
    let minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
    let second = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

    if (year < 2018) {
      return false;
    }

    // 如果是2018年,12月，那么要15号并且17点之后显示
    if (year === 2018 && month === 12) {
      return day >= 15 && hour >= 17;
    }
    else {
      return true;
    }
  }
}

