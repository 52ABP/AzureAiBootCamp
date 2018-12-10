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

}

