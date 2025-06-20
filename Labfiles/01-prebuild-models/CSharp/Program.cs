﻿using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

// Store connection information
string endpoint = "https://learning-docuemnt-intelligence.cognitiveservices.azure.com/";
string apiKey = "D4Smb6onpETjUivJOB4zfRU4yBqzhVPvzatvSb6qrP32oVcAuTSWJQQJ99BFACYeBjFXJ3w3AAALACOGHwGT";

Uri fileUri = new Uri("https://github.com/MicrosoftLearning/mslearn-ai-document-intelligence/blob/main/Labfiles/01-prebuild-models/sample-invoice/sample-invoice.pdf?raw=true");

Console.WriteLine("\nConnecting to Forms Recognizer at: {0}", endpoint);
Console.WriteLine("Analyzing invoice at: {0}\n", fileUri.ToString());

// Create the client
var azureCardential = new AzureKeyCredential(apiKey);
var client = new DocumentAnalysisClient(new Uri(endpoint), azureCardential);

// Analyze the invoice
AnalyzeDocumentOperation operation = await client.AnalyzeDocumentFromUriAsync(
    WaitUntil.Completed,
    "prebuilt-invoice",
    fileUri);

// Display invoice information to the user
AnalyzeResult result = operation.Value;

foreach (AnalyzedDocument invoice in result.Documents)
{
    if (invoice.Fields.TryGetValue("VendorName", out DocumentField? vendorNameField))
    {
        if (vendorNameField.FieldType == DocumentFieldType.String)
        {
            string vendorName = vendorNameField.Value.AsString();
            Console.WriteLine($"Vendor Name: '{vendorName}', with confidence {vendorNameField.Confidence}.");
        }
    }
    
    if (invoice.Fields.TryGetValue("CustomerName", out DocumentField? customerNameField))
    {
        if (customerNameField.FieldType == DocumentFieldType.String)
        {
            string customerName = customerNameField.Value.AsString();
            Console.WriteLine($"Customer Name: '{customerName}', with confidence {customerNameField.Confidence}.");
        }
    }

    if (invoice.Fields.TryGetValue("InvoiceTotal", out DocumentField? invoiceTotalField))
    {
        if (invoiceTotalField.FieldType == DocumentFieldType.Currency)
        {
            CurrencyValue invoiceTotal = invoiceTotalField.Value.AsCurrency();
            Console.WriteLine($"Invoice Total: '{invoiceTotal.Symbol}{invoiceTotal.Amount}', with confidence {invoiceTotalField.Confidence}.");
        }
    }
}

Console.WriteLine("\nAnalysis complete.\n");