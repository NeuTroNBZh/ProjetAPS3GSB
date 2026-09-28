Imports System.IO
Imports GSB.CR.Metier
Imports GSB.CR.Modeles

''' <summary>
''' Enregistrement d'une synthèse d'activité en CSV (EX-51) : choix du fichier, écriture, message de confirmation.
''' Utilisé par « Mon activité », « Ma région » et « Mon secteur ».
''' </summary>
Public Module EnregistrementExport

    Public Sub Exporter(proprietaire As IWin32Window, perimetre As String, synthese As SyntheseActivite,
                        Optional membres As IEnumerable(Of ActiviteMembre) = Nothing)
        Using dlg As New SaveFileDialog() With {
            .Title = "Exporter les statistiques",
            .Filter = "Fichier CSV (Excel)|*.csv",
            .FileName = ExportStatistiques.NomDeFichier(perimetre, synthese.Debut, synthese.Fin),
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            .OverwritePrompt = True}
            If dlg.ShowDialog(proprietaire) <> DialogResult.OK Then Return
            Try
                File.WriteAllText(dlg.FileName, ExportStatistiques.Generer(perimetre, synthese, membres, DateTime.Now), ExportStatistiques.Encodage)
                MessageBox.Show(proprietaire, $"Statistiques exportées dans :{Environment.NewLine}{dlg.FileName}",
                                "GSB - Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception When TypeOf ex Is IOException OrElse TypeOf ex Is UnauthorizedAccessException
                MessageBox.Show(proprietaire, $"Impossible d'écrire le fichier : {ex.Message}{Environment.NewLine}" &
                                "Vérifiez qu'il n'est pas ouvert dans Excel, ou choisissez un autre dossier.",
                                "GSB - Export", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Using
    End Sub

End Module
