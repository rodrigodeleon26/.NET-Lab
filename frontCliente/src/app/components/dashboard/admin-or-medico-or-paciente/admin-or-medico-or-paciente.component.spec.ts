import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminOrMedicoOrPacienteComponent } from './admin-or-medico-or-paciente.component';

describe('AdminOrMedicoOrPacienteComponent', () => {
  let component: AdminOrMedicoOrPacienteComponent;
  let fixture: ComponentFixture<AdminOrMedicoOrPacienteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminOrMedicoOrPacienteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminOrMedicoOrPacienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
