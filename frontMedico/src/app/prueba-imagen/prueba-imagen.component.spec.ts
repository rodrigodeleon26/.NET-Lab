import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PruebaImagenComponent } from './prueba-imagen.component';

describe('PruebaImagenComponent', () => {
  let component: PruebaImagenComponent;
  let fixture: ComponentFixture<PruebaImagenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PruebaImagenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PruebaImagenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
