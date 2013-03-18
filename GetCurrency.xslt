<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xml:space="default">
  <xsl:output method="text" indent="no"/>

  <xsl:template match="tbody">
    <xsl:apply-templates select="tr">
      <xsl:sort select="td[4]"/>
    </xsl:apply-templates>
  </xsl:template>
  
  <xsl:template match="tr">_currencies[<xsl:apply-templates select="td[4]" />] = new CurrencyTableEntry("<xsl:apply-templates select="td[2]" />", "<xsl:apply-templates select="td[3]" />", <xsl:apply-templates select="td[4]" />, lookupString("<xsl:apply-templates select="td[3]" />", symbolLookup));
  </xsl:template>
  <xsl:template match="td" xml:space="default"><xsl:value-of select="normalize-space(node())" /></xsl:template>
</xsl:stylesheet>
