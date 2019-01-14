import React from 'react'

import Container from 'react-bulma-components/lib/components/container'
import Content from 'react-bulma-components/lib/components/content'
import Footer from 'react-bulma-components/lib/components/footer'
import Heading from 'react-bulma-components/lib/components/heading'
import Section from 'react-bulma-components/lib/components/section'

const Layout = (props) => (
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
      </Container>
    </Footer>
  </div>
)

export default Layout
