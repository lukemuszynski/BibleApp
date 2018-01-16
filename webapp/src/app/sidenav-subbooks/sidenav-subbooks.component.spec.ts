import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavSubbooksComponent } from './sidenav-subbooks.component';

describe('SidenavSubbooksComponent', () => {
  let component: SidenavSubbooksComponent;
  let fixture: ComponentFixture<SidenavSubbooksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SidenavSubbooksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SidenavSubbooksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
