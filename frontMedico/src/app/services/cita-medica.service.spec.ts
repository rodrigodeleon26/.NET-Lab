import { TestBed } from '@angular/core/testing';

import { CitasMedicasService } from './cita-medica.service';

describe('CitasMedicasService', () => {
  let service: CitasMedicasService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CitasMedicasService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
