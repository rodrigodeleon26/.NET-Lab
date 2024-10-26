import { TestBed } from '@angular/core/testing';

import { ConsultaMedicaService } from './consulta-medica.service';

describe('ConsultaMedicaService', () => {
  let service: ConsultaMedicaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConsultaMedicaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
