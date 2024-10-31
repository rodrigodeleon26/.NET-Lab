import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEspecialidadComponent } from './add-especialidad.component';

describe('AddEspecialidadComponent', () => {
  let component: AddEspecialidadComponent;
  let fixture: ComponentFixture<AddEspecialidadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddEspecialidadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEspecialidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
