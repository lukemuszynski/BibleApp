import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
// tslint:disable-next-line:max-line-length
import { MatCheckboxModule, MatButtonModule, MatChipsModule, MatSidenavModule, MatListModule, MatToolbarModule, MatIconModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatMenuModule } from '@angular/material';
import { MatExpansionModule } from '@angular/material';
import { MatDatepickerModule } from '@angular/material';
import { MatNativeDateModule } from '@angular/material';
import { MatTabsModule } from '@angular/material';
import { MatCardModule } from '@angular/material';
import { MatGridListModule } from '@angular/material';
import { MatFormFieldModule, MatSelectModule } from '@angular/material';
@NgModule({
  imports: [
    MatButtonModule,
    MatCheckboxModule,
    MatChipsModule,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatIconModule,
    MatInputModule,
    FlexLayoutModule,
    MatMenuModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatCardModule,
    MatGridListModule,
    MatFormFieldModule,
    MatSelectModule
  ],
  exports: [
    BrowserAnimationsModule,
    MatButtonModule,
    MatCheckboxModule,
    MatChipsModule,
    MatSidenavModule,
    MatListModule,
    MatToolbarModule,
    MatIconModule,
    MatInputModule,
    FlexLayoutModule,
    MatMenuModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatCardModule,
    MatGridListModule,
    MatFormFieldModule,
    MatSelectModule
  ],
  declarations: []
})
export class CustomMaterialModule { }
