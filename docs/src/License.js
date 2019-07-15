import React from 'react'
import { withTranslation } from 'react-i18next'

import Container from 'react-bulma-components/lib/components/container'
import Heading from 'react-bulma-components/lib/components/heading'

import Layout from './Layout'

const License = ({ t }) => (
  <Layout title={ t('license.title') }>

    <Container>
      <Heading>
        <a href="https://github.com/LitJSON/litjson">LitJson</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
        JSON library for the .Net framework. Unlicense
      </div>
    </Container>

    <Container>
      <Heading>
        <a href="http://www.pixelplacement.com/itween/index.php">iTween</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
        iTween is a simple, powerful and easy to use animation system for Unity. iTween is a free, open-source project. As such, it doesn't require any purchase or licensing to be used on commercial projects or otherwise.
      </div>
    </Container>

    <Container>
      <Heading>
        <a href="http://narudesign.com/devlog/unity-create-simple-plane/">CreateSimplePlane</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
      </div>
    </Container>

    <Container>
      <Heading>
        <a href="http://itouhiro.hatenablog.com/entry/20130602/font">PixelMplus</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
        Copyright (C) 2002-2013 M+ FONTS PROJECT / Itou Hiroki
      </div>
    </Container>

    <Container>
      <Heading>
        <a href="https://fonts2u.com/gaufontpopmagic.font">GauFontPopMagic</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
        Copyright notice Nov. 2000. Graphic Arts Unit Private Site "GAU+" Ver.1.20
      </div>
    </Container>

    <Container>
      <Heading>
        <a href="http://amachamusic.chagasi.com/">甘茶の音楽工房</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
      </div>
    </Container>

    <Container>
      <Heading>
        <a href="https://dova-s.jp/">DOVA-SYNDROME</a>
      </Heading>
      <div style={{ 'whiteSpace': 'pre-wrap' }}>
      </div>
    </Container>

  </Layout>
)

export default withTranslation()(License)
