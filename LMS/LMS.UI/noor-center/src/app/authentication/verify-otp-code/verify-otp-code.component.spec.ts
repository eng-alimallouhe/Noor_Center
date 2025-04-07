import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyOtpCodeComponent } from './verify-otp-code.component';

describe('VerifyOtpCodeComponent', () => {
  let component: VerifyOtpCodeComponent;
  let fixture: ComponentFixture<VerifyOtpCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerifyOtpCodeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyOtpCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
