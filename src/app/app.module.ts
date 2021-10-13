import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { MainComponent } from './main/main.component';
import { InputComponent } from './main/input/input.component';
import { ListComponent } from './main/list/list.component';
import { ItemComponent } from './main/list/item/item.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    MainComponent,
    InputComponent,
    ListComponent,
    ItemComponent
  ],
  imports: [
    BrowserModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
