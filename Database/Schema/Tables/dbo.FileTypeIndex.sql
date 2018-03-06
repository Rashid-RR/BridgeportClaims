CREATE TABLE [dbo].[FileTypeIndex]
(
[FileTypeID] [tinyint] NOT NULL,
[InvoiceNumber] [int] NOT NULL
) ON [PRIMARY]
WITH
(
DATA_COMPRESSION = ROW
)
GO
ALTER TABLE [dbo].[FileTypeIndex] ADD CONSTRAINT [pkFileTypeIndex] PRIMARY KEY CLUSTERED  ([FileTypeID]) WITH (FILLFACTOR=90, DATA_COMPRESSION = ROW) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FileTypeIndex] ADD CONSTRAINT [fkFileTypeIndexFileTypeIDFileTypeFileTypeID] FOREIGN KEY ([FileTypeID]) REFERENCES [dbo].[FileType] ([FileTypeID])
GO
