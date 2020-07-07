Partial Public Class offLine
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Select Case CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

      Case eWebHostTypes.ADMIN
        logo.ImageUrl = "~/images/JETNETEvolutionAdminFINAL.png" 'swap logo
        Page.Header.Title = "Evolution Administrator Maintenance"
        background_image.ImageUrl = "~/images/background/21.jpg"
        logo.CssClass = "logo_image"
        welcome_to_text.Text = "Welcome to JETNET Evolution Administration"
        welcome_paragraph.Text = "Evolution Administrator is temporarily down for maintenance. If you have any questions contact <a href=""mailto:customerservice@jetnet.com"">customerservice@jetnet.com</a> or 1-(800)-553-8638 FREE"

      Case eWebHostTypes.ABI
        logo.ImageUrl = "~/abiFiles/images/abiLogo.png" 'swap logo
        Page.Header.Title = "Jetnet Global Maintenance"
        background_image.ImageUrl = ""
        logo.CssClass = "logo_image"
        welcome_to_text.Text = "Welcome to jetnetGlobal"
        welcome_paragraph.Text = "jetnetGlobal is temporarily down for maintenance. If you have any questions contact <a href=""mailto:customerservice@jetnet.com"">customerservice@jetnet.com</a> or 1-(800)-553-8638 FREE"

      Case eWebHostTypes.CRM
        logo.ImageUrl = "~/images/market_manager.png" 'swap logo out
        Page.Header.Title = "Marketplace Manager Maintenance"
        background_image.ImageUrl = "~/images/background/10.jpg"
        logo.CssClass = "logo_image"
        welcome_to_text.Text = "Welcome to Evolution Marketplace Manager"
        welcome_paragraph.Text = "Marketplace Manager is temporarily down for maintenance. If you have any questions contact <a href=""mailto:customerservice@jetnet.com"">customerservice@jetnet.com</a> or 1-(800)-553-8638 FREE"

      Case eWebHostTypes.YACHT
        logo.ImageUrl = "~/images/YachtSpot_Logo.png" 'swap logo
        Page.Header.Title = "Yacht Spot Online Maintenance"
        background_image.ImageUrl = "~/images/background/31.jpg"
        logo.CssClass = "logo_image"
        welcome_to_text.Text = "Welcome to YachtSpot Online"
        welcome_paragraph.Text = "Yacht Spot Online is temporarily down for maintenance. If you have any questions contact <a href=""mailto:customerservice@jetnet.com"">customerservice@jetnet.com</a> or 1-(800)-553-8638 FREE"

      Case Else
        logo.ImageUrl = "~/images/JN_EvolutionMarketplace_Logo2.png" 'swap logo
        Page.Header.Title = "JETNET Evolution Maintenance"
        background_image.ImageUrl = "~/images/background/11.jpg"
        logo.CssClass = "logo_image"
        welcome_to_text.Text = "Welcome to JETNET Evolution"
        welcome_paragraph.Text = "JETNET Evolution is temporarily down for maintenance. If you have any questions contact <a href=""mailto:customerservice@jetnet.com"">customerservice@jetnet.com</a> or 1-(800)-553-8638 FREE"

    End Select

  End Sub

End Class