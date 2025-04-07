import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'noor-center';

  constructor(private translate: TranslateService) {
    const browserLang = navigator.language.split('-')[0];
    const supportedLangs = ['en', 'ar'];

    console.log(browserLang)

    console.log(supportedLangs)

    const defaultLang = supportedLangs.includes(browserLang) ? browserLang : 'en';

    translate.setDefaultLang(defaultLang);

    translate.use(defaultLang);

    this.translate.onLangChange.subscribe((event) => {
      this.setDirection(event.lang);
    });

  }


  setDirection(lang: string) {
    if (lang === 'ar') {
      this.loadRTLStylesheet();
    } else {
      this.removeRTLStylesheet();
    }
  }

  loadRTLStylesheet() {
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = 'assets/styles/rtl-styles.css';
    document.head.appendChild(link);
  }

  removeRTLStylesheet() {
    const links = document.head.getElementsByTagName('link');
    for (let i = 0; i < links.length; i++) {
      const link = links[i];
      if (link.href && link.href.includes('rtl-styles.css')) {
        document.head.removeChild(link);
      }
    }
  }

}
