<!--<?xml version="1.0" encoding="utf-8"?>-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <body>
        <h1>Catalogue</h1>
        <xsl:for-each select="/catalogue/album">
          <h2>Album: 
            <xsl:value-of select="name"/>
          </h2>
          <h3>Artist: 
            <xsl:value-of select="artist"/>
          </h3>
          <div> Year: 
            <xsl:value-of select="year"/>
          </div>
          <div>
            Producer: 
            <xsl:value-of select="producer"/>
          </div>
          <div>
            Price: $
            <xsl:value-of select="price"/>
          </div>
          <div>
            Songs:
            <xsl:for-each select="song">
              <div>
                Title: 
                <xsl:value-of select="title"/>
                 Duration: 
                <xsl:value-of select="duration"/>
                 minutes
              </div>
            </xsl:for-each>
          </div>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>