import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistorialFacturacionComponent } from './historial-facturacion.component';

describe('HistorialFacturacionComponent', () => {
  let component: HistorialFacturacionComponent;
  let fixture: ComponentFixture<HistorialFacturacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HistorialFacturacionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistorialFacturacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
