import React from 'react'
import i18next from 'i18next'

import Container from 'react-bulma-components/lib/components/container'
import Content from 'react-bulma-components/lib/components/content'
import Footer from 'react-bulma-components/lib/components/footer'
import Heading from 'react-bulma-components/lib/components/heading'
import Section from 'react-bulma-components/lib/components/section'
import Tile from 'react-bulma-components/lib/components/tile'

const Layout = () => (
  <div>
	  <Section>

	    <Container>
	      <Heading>{ i18next.t('privacy.title') }</Heading>

	      <Tile notification color="primary">

	      </Tile>
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
