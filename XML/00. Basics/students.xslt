<!--<?xml version="1.0" encoding="utf-8"?>-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <body>
        <h1>Telerik Students</h1>
        <table bgcolor="#E0E0E0" cellspacing="1">
          <tr bgcolor="#EEEEEE">
            <td>
              <b>Name</b>
            </td>
            <td>
              <b>Faculty number</b>
            </td>
            <td>
              <b>Course</b>
            </td>
          </tr>
          <xsl:for-each select="/student-system/telerik:student">
            <tr bgcolor="white">
              <td>
                <xsl:value-of select="telerik:student-name"/>
              </td>
              <td>
                <xsl:value-of select="telerik:faculty-number"/>
              </td>
              <td>
                <xsl:value-of select="telerik:course"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>