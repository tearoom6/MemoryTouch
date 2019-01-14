import React from 'react'
import ReactDOM from 'react-dom'
import i18next from 'i18next'
import Backend from 'i18next-xhr-backend'
import LngDetector from 'i18next-browser-languagedetector'
import WebFont from 'webfontloader'

import Privacy from './Privacy'

WebFont.load({
  google: {
    families: ['M PLUS 1p', 'Montserrat']
  }
})

i18next
  .use(LngDetector)
  .use(Backend)
  .init({
    fallbackLng: 'en',
    backend: {
      loadPath: '/locales/{{lng}}.json',
    },
  }, function(err, t) {
    // i18n initialized.
    ReactDOM.render(
      <Privacy/>,
      document.getElementById('root')
    )
  })
