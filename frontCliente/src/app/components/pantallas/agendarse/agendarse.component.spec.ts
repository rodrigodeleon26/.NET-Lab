import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgendarseComponent } from './agendarse.component';

describe('AgendarseComponent', () => {
  let component: AgendarseComponent;
  let fixture: ComponentFixture<AgendarseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AgendarseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AgendarseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
