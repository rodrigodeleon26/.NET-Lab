import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerarMedicoComponent } from './generar-medico.component';

describe('GenerarMedicoComponent', () => {
  let component: GenerarMedicoComponent;
  let fixture: ComponentFixture<GenerarMedicoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GenerarMedicoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenerarMedicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
