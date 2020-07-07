Public Partial Class subscriptionInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Fill_Drop()

    If Application.Item("DebugFlag") Or Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.TEST Or Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then
      change_subscription.Visible = True
      change_subscription2.Visible = True
    Else
      change_subscription.Visible = False
      change_subscription2.Visible = False
    End If

        Dim text As String = "<table width='100%' cellpadding='0' cellspacing='0'>"
        text = text & "<tr><td align='left' valign='top' width='120'>Name:</td><td align='left' valign='top'>" & Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName & "</td></tr>"
        text = text & "<tr><td align='left' valign='top' width='120'>Email Address:</td><td align='left' valign='top'>" & Session.Item("localUser").crmLocalUserEmailAddress & "</td></tr>"
        text = text & "<tr><td align='left' valign='top'>Product(s):</td><td align='left' valign='top'>"
        If Session.Item("localSubscription").crmHelicopter_Flag Then
            text = text & "Helicopters,"
        End If

        If Session.Item("localSubscription").crmBusiness_Flag Then
            text = text & " Business,"
        End If
        If Session.Item("localSubscription").crmCommercial_Flag Then
            text = text & " Commercial,"
        End If

        text = text.TrimEnd(",")
        text = text & "</td></tr>"

        text = text & "<tr><td align='left' valign='top'>Tier(s):</td><td align='left' valign='top'>"

        If Session.Item("localSubscription").crmTurboprops Then
            text = text & "Turboprops,"
        End If

        If Session.Item("localSubscription").crmExecutive_Flag Then
            text = text & " Executive,"
        End If
        If Session.Item("localSubscription").crmJets_Flag Then
            text = text & " Jets,"
        End If

        text = text.TrimEnd(",")

        text = text & " <em>" & Session.Item("localSubscription").crmTierlevel & "</em>"
        text = text & "</td></tr>"

        text = text & "<tr><td align='left' valign='top'></td><td align='left' valign='top'>" & IIf(Session.Item("localSubscription").crmAerodexFlag = True, "Aerodex User", "Marketplace User") & "</td></tr>"

        text = text & "<tr><td align='left' valign='top'>Frequency: </td><td align='left' valign='top'>" & Session.Item("localSubscription").crmFrequency & "</td></tr>"

        text = text & "<tr><td colspan='2'><hr /></td></tr>"

        text = text & "<tr><td align='left' valign='top'>Star Reports: </td><td align='left' valign='top'>" & IIf(Session.Item("localSubscription").crmStar_Reports_Flag = False, "No", "Yes") & "</td></tr>"
        text = text & "<tr><td align='left' valign='top'>Sales Price Index: </td><td align='left' valign='top'>" & IIf(Session.Item("localSubscription").crmSalesPriceIndex_Flag = False, "No", "Yes") & "</td></tr>"
        text = text & "<tr><td align='left' valign='top'>Server Side Notes: </td><td align='left' valign='top'>" & IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = False, "No", "Yes") & "</td></tr>"

        text = text & "<tr><td align='left' valign='top'>Evo Display: </td><td align='left' valign='top'>" & IIf(Session.Item("localUser").crmEvo = False, "No", "Yes") & "</td></tr></table>"


        subscription_Information.Text = text





    End Sub

    Private Sub Fill_Drop()

        'load drop down if not post back
        If Not Page.IsPostBack Then
            '------aero
            If Not IsNothing(Session.Item("localSubscription").crmAerodexFlag) Then
                If Session.Item("localSubscription").crmAerodexFlag = True Then
                    aerodex_session.SelectedValue = "true"
                Else
                    aerodex_session.SelectedValue = "false"
                End If
            End If

            '-------product
            If Not IsNothing(Session.Item("localSubscription").crmHelicopter_Flag) Then
                If Session.Item("localSubscription").crmHelicopter_Flag = True Then
                    helicopter_session.SelectedValue = "true"
                Else
                    helicopter_session.SelectedValue = "false"
                End If
            End If


            If Not IsNothing(Session.Item("localSubscription").crmBusiness_Flag) Then
                If Session.Item("localSubscription").crmBusiness_Flag = True Then
                    business_session.SelectedValue = "true"
                Else
                    business_session.SelectedValue = "false"
                End If
            End If


            If Not IsNothing(Session.Item("localSubscription").crmCommercial_Flag) Then
                If Session.Item("localSubscription").crmCommercial_Flag = True Then
                    commercial_session.SelectedValue = "true"
                Else
                    commercial_session.SelectedValue = "false"
                End If
            End If


            '---tiers



            If Not IsNothing(Session.Item("localSubscription").crmTurboprops) Then
                If Session.Item("localSubscription").crmTurboprops = True Then
                    turboprops_session.SelectedValue = "true"
                Else
                    turboprops_session.SelectedValue = "false"
                End If
            End If

            If Not IsNothing(Session.Item("localSubscription").crmExecutive_Flag) Then
                If Session.Item("localSubscription").crmExecutive_Flag = True Then
                    executive_session.SelectedValue = "true"
                Else
                    executive_session.SelectedValue = "false"
                End If
            End If

            If Not IsNothing(Session.Item("localSubscription").crmJets_Flag) Then
                If Session.Item("localSubscription").crmJets_Flag = True Then
                    jets_session.SelectedValue = "true"
                Else
                    jets_session.SelectedValue = "false"
                End If
            End If



            'star reports

            If Not IsNothing(Session.Item("localSubscription").crmStar_Reports_Flag) Then
                If Session.Item("localSubscription").crmStar_Reports_Flag = True Then
                    star_reports_session.SelectedValue = "true"
                Else
                    star_reports_session.SelectedValue = "false"
                End If
            End If

            'frequency
            If Not IsNothing(Session.Item("localSubscription").crmFrequency) Then
                If Session.Item("localSubscription").crmFrequency <> "" Then
                    frequency_session.SelectedValue = Session.Item("localSubscription").crmFrequency
                End If
            End If


            'SPI

            If Not IsNothing(Session.Item("localSubscription").crmSalesPriceIndex_Flag) Then
                If Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                    sales_price_session.SelectedValue = "true"
                Else
                    sales_price_session.SelectedValue = "false"
                End If
            End If

            'Evo Display

            If Not IsNothing(Session.Item("localUser").crmEvo) Then
                If Session.Item("localUser").crmEvo = True Then
                    evo_session.SelectedValue = "true"
                Else
                    evo_session.SelectedValue = "false"
                End If
            End If

            'ssn

            If Not IsNothing(Session.Item("localSubscription").crmServerSideNotes_Flag) Then
                If Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                    server_side_notes_session.SelectedValue = "true"
                Else
                    server_side_notes_session.SelectedValue = "false"
                End If
            End If

        End If

    End Sub




    Private Sub Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit.Click
        Dim product_code As String = ""
        Dim tier As String = ""
        '------aero
        If aerodex_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmAerodexFlag = True
        Else
            Session.Item("localSubscription").crmAerodexFlag = False
        End If


        '-------product
        If helicopter_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmHelicopter_Flag = True
            product_code = "H,"
        Else
            Session.Item("localSubscription").crmHelicopter_Flag = False
        End If


        If business_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmBusiness_Flag = True
            product_code = product_code & "B,"
        Else
            Session.Item("localSubscription").crmBusiness_Flag = False
        End If

        If commercial_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmCommercial_Flag = True
            product_code = product_code & "C,"
        Else
            Session.Item("localSubscription").crmCommercial_Flag = False
        End If

        product_code = product_code.Trim(",")
        Session.Item("localSubscription").crmProductCode = product_code
        '---tiers
        If turboprops_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmTurboprops = True
            tier = tier & "T,"
        Else
            Session.Item("localSubscription").crmTurboprops = False
        End If

        If executive_session.SelectedValue = "true" Then
            tier = tier & "E,"
            Session.Item("localSubscription").crmExecutive_Flag = True
        Else
            Session.Item("localSubscription").crmExecutive_Flag = False
        End If

        If jets_session.SelectedValue = "true" Then
            tier = tier & "J,"
            Session.Item("localSubscription").crmJets_Flag = True
        Else
            Session.Item("localSubscription").crmJets_Flag = False
        End If

        If jets_session.SelectedValue = "true" And executive_session.SelectedValue = "true" And turboprops_session.SelectedValue = "true" Then
            tier = "ALL"
        End If
        tier = tier.Trim(",")
        Session.Item("localSubscription").crmTierlevel = tier
        'star reports
        If star_reports_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmStar_Reports_Flag = True
        Else
            Session.Item("localSubscription").crmStar_Reports_Flag = False
        End If

        'frequency
        If frequency_session.SelectedValue <> "" Then
            Session.Item("localSubscription").crmFrequency = frequency_session.SelectedValue
        End If


        'SPI
        If sales_price_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmSalesPriceIndex_Flag = True
        Else
            Session.Item("localSubscription").crmSalesPriceIndex_Flag = False
        End If

        'ssn
        If server_side_notes_session.SelectedValue = "true" Then
            Session.Item("localSubscription").crmServerSideNotes_Flag = True
        Else
            Session.Item("localSubscription").crmServerSideNotes_Flag = False
        End If

        'evo
        If evo_session.SelectedValue = "true" Then
            Session.Item("localUser").crmEvo = True
        Else
            Session.Item("localUser").crmEvo = False
        End If


        Response.Redirect("subscriptioninfo.aspx")

    End Sub
End Class