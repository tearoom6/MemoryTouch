import React from 'react'

import i18next from 'i18next'

import Container from 'react-bulma-components/lib/components/container'
import Content from 'react-bulma-components/lib/components/content'
import Dropdown from 'react-bulma-components/lib/components/dropdown'
import Footer from 'react-bulma-components/lib/components/footer'
import Heading from 'react-bulma-components/lib/components/heading'
import Section from 'react-bulma-components/lib/components/section'

const Layout = (props) => {
  const changeLanguage = (lang) => {
    i18next.changeLanguage(lang)
  }
  return (
    <div>
      <Section>

        <Container>
          <Heading>{ props.title }</Heading>

          { props.children }
        </Container>

      </Section>

      <Footer>
        <Container>
          <Content style={{ textAlign: 'center' }}>
            <p>
              <strong>Breakthrough</strong> by <a href="https://tearoom6.github.io/">tearoom6</a>. All rights reserved.
            </p>
          </Content>
          <Dropdown value={ i18next.language } onChange={ changeLanguage }>
            <Dropdown.Item value="en">English</Dropdown.Item>
            <Dropdown.Item value="ja">日本語</Dropdown.Item>
          </Dropdown>
        </Container>
      </Footer>
    </div>
  )
}

export default Layout
