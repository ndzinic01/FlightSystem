import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Country, CountryAddUpdate, CountryService } from '../../Services/country.service';


@Component({
  selector: 'app-countries',
  standalone: false,
  templateUrl: './countries.html',
  styleUrls: ['./countries.css']
})
export class Countries implements OnInit {

  countries: Country[] = [];
  form!: FormGroup;
  selectedId: number | null = null;

  constructor(
    private service: CountryService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadCountries();
  }

  initForm() {
    this.form = this.fb.group({
      name: ['', Validators.required]
    });
  }

  loadCountries() {
    this.service.getAll().subscribe(res => {
      this.countries = res;
    });
  }

  edit(country: Country) {
    this.selectedId = country.id;
    this.form.patchValue(country);
  }

  delete(id: number) {
    if (!confirm('Delete this country?')) return;
    this.service.delete(id).subscribe(() => this.loadCountries());
  }

  save() {
    const dto: CountryAddUpdate = this.form.value;

    if (this.selectedId) {
      this.service.update(this.selectedId, dto).subscribe(() => {
        this.reset();
        this.loadCountries();
      });
    } else {
      this.service.create(dto).subscribe(() => {
        this.reset();
        this.loadCountries();
      });
    }
  }

  reset() {
    this.selectedId = null;
    this.form.reset();
  }
}

