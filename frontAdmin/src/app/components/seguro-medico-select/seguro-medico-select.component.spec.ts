import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeguroMedicoSelectComponent } from './seguro-medico-select.component';

describe('SeguroMedicoSelectComponent', () => {
  let component: SeguroMedicoSelectComponent;
  let fixture: ComponentFixture<SeguroMedicoSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SeguroMedicoSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeguroMedicoSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
