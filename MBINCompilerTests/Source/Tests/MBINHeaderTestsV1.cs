using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace libMBIN.UnitTests {

    using MBIN;

    [TestClass]
    public class MBINHeaderTestsV1 {

        private const string NULL_VERSION_STRING = "0.0.0.0";

        private static readonly string VERSION_STRING = Version.AssemblyVersion.ToString( 3 );
        private static readonly string VERSION_STRING_PADDED = VERSION_STRING.PadRight( 8, '\0' );

        private static readonly ulong  VERSION_ID = MbinHeader.StringToUlong( VERSION_STRING_PADDED );

        private const string TEMPLATE_NAME = "templateName";
        private const ulong  END_PADDING   = 0ul;


        private const ulong  TIMESTAMP     = 201809071631;
        private const ulong  TEMPLATE_GUID = 0xF1E2D3C4B5A6978;

        private MbinHeader CreateMockHeader(
                    uint magic    = MbinHeader.MBIN_MAGIC,
                    uint formatID  = MbinHeader.MBIN_VERSION,
                    ulong tag     = MbinHeader.MBINCVER_TAG,
                    ulong version = ~0ul,
                    string name   = TEMPLATE_NAME,
                    ulong padding = END_PADDING
        ) {
            return new MbinHeader() {
                MagicID         = magic,
                FormatID      = formatID,
                Tag           = tag,
                MbinVersion   = (version == ~0ul) ? VERSION_ID : version,
                TemplateName  = name,
                EndPadding    = padding
            };
        }

        private MbinHeader HeaderCommon => CreateMockHeader();
        private MbinHeader HeaderTkGeometryData => CreateMockHeader( magic: MbinHeader.MBIN_MAGIC_PC );
        private MbinHeader HeaderTkAnimMetadata => CreateMockHeader(
                    tag:       MbinHeader.TKANIMMETADATA_TAG,
                    version:   MbinHeader.TKANIMMETADATA_VERSION,
                    padding:   MbinHeader.TKANIMMETADATA_PADDING
        );

        [TestMethod]
        public void TestIsValid() {
            Assert.IsTrue( HeaderCommon.IsValid );
            Assert.IsTrue( HeaderTkGeometryData.IsValid );
            Assert.IsTrue( HeaderTkAnimMetadata.IsValid );
        }

        [TestMethod]
        public void TestGetFormat() {
            Assert.AreEqual( 1, HeaderCommon.GetFormat() );
            Assert.AreEqual( 1, HeaderTkGeometryData.GetFormat() );
            Assert.AreEqual( 0, HeaderTkAnimMetadata.GetFormat() );
        }

        [TestMethod]
        public void TestIsFormatV0() {
            Assert.IsFalse( HeaderCommon.IsFormatV0 );
            Assert.IsFalse( HeaderTkGeometryData.IsFormatV0 );
            Assert.IsTrue(  HeaderTkAnimMetadata.IsFormatV0 );
        }

        [TestMethod]
        public void TestIsFormatV1() {
            Assert.IsTrue(  HeaderCommon.IsFormatV1 );
            Assert.IsTrue(  HeaderTkGeometryData.IsFormatV1 );
            Assert.IsFalse( HeaderTkAnimMetadata.IsFormatV1 );
        }

        [TestMethod]
        public void TestIsFormatV2() {
            Assert.IsFalse( HeaderCommon.IsFormatV2 );
            Assert.IsFalse( HeaderTkGeometryData.IsFormatV2 );
            Assert.IsFalse( HeaderTkAnimMetadata.IsFormatV2 );
        }

        [TestMethod]
        public void TestStringToUlong() {
            Assert.AreEqual( VERSION_ID, MbinHeader.StringToUlong( VERSION_STRING_PADDED ) );
        }

        [TestMethod]
        public void TestUlongToString() {
            Assert.AreEqual( VERSION_STRING_PADDED, MbinHeader.UlongToString( VERSION_ID ) );
        }

        [TestMethod]
        public void TestCreateHeaderCommon() {
            Assert.AreEqual( MbinHeader.MBIN_MAGIC,   HeaderCommon.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION, HeaderCommon.FormatID );
            Assert.AreEqual( MbinHeader.MBINCVER_TAG, HeaderCommon.Timestamp );
            Assert.AreEqual( VERSION_ID,           HeaderCommon.TemplateGUID );
            Assert.AreEqual( TEMPLATE_NAME,           HeaderCommon.TemplateName );
            Assert.AreEqual( END_PADDING,             HeaderCommon.EndPadding );

            Assert.AreEqual( VERSION_STRING,          HeaderCommon.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestCreateHeaderTkGeometryData() {
            Assert.AreEqual( MbinHeader.MBIN_MAGIC_PC, HeaderTkGeometryData.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION,  HeaderTkGeometryData.FormatID );
            Assert.AreEqual( MbinHeader.MBINCVER_TAG,  HeaderTkGeometryData.Timestamp );
            Assert.AreEqual( VERSION_ID,            HeaderTkGeometryData.TemplateGUID );
            Assert.AreEqual( TEMPLATE_NAME,            HeaderTkGeometryData.TemplateName );
            Assert.AreEqual( END_PADDING,              HeaderTkGeometryData.EndPadding );

            Assert.AreEqual( VERSION_STRING,          HeaderTkGeometryData.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestCreateHeaderTkAnimMetadata() {
            Assert.AreEqual( MbinHeader.MBIN_MAGIC,             HeaderTkAnimMetadata.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION,           HeaderTkAnimMetadata.FormatID );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_TAG,     HeaderTkAnimMetadata.Timestamp );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_VERSION, HeaderTkAnimMetadata.TemplateGUID );
            Assert.AreEqual( TEMPLATE_NAME,                     HeaderTkAnimMetadata.TemplateName );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_PADDING, HeaderTkAnimMetadata.EndPadding );
        }

        [TestMethod]
        public void TestSetDefaultsV1Common() {
            var header = new MbinHeader();
            header.SetDefaultsV1();

            Assert.AreEqual( MbinHeader.MBIN_MAGIC,   header.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION, header.FormatID );
            Assert.AreEqual( MbinHeader.MBINCVER_TAG, header.Tag );
            Assert.AreEqual( VERSION_ID,              header.MbinVersion );
            Assert.AreEqual( "",                      header.TemplateName );
            Assert.AreEqual( END_PADDING,             header.EndPadding );

            Assert.AreEqual( VERSION_STRING,          header.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestSetDefaultsV1TkGeometry() {
            var header = new MbinHeader();
            header.SetDefaultsV1( typeof( NMS.Toolkit.TkGeometryData ) );

            Assert.AreEqual( MbinHeader.MBIN_MAGIC_PC, header.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION,  header.FormatID );
            Assert.AreEqual( MbinHeader.MBINCVER_TAG,  header.Tag );
            Assert.AreEqual( VERSION_ID,               header.MbinVersion );
            Assert.AreEqual( "",                       header.TemplateName );
            Assert.AreEqual( END_PADDING,              header.EndPadding );

            Assert.AreEqual( VERSION_STRING,           header.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestSetDefaultsV1TkAnimMetadata() {
            var header = new MbinHeader();
            header.SetDefaultsV1( typeof( NMS.Toolkit.TkAnimMetadata ) );

            Assert.AreEqual( MbinHeader.MBIN_MAGIC,             header.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION,           header.FormatID );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_TAG,     header.Tag );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_VERSION, header.MbinVersion );
            Assert.AreEqual( "",                                header.TemplateName );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_PADDING, header.EndPadding );

            Assert.AreEqual( NULL_VERSION_STRING,               header.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestSetDefaultsCommon() {
            var header = new MbinHeader();
            header.SetDefaults( format: MbinHeader.Format.V1 );

            Assert.AreEqual( MbinHeader.MBIN_MAGIC,   header.MagicID      );
            Assert.AreEqual( MbinHeader.MBIN_VERSION, header.FormatID     );
            Assert.AreEqual( MbinHeader.MBINCVER_TAG, header.Tag          );
            Assert.AreEqual( VERSION_ID,              header.MbinVersion  );
            Assert.AreEqual( "",                      header.TemplateName );
            Assert.AreEqual( END_PADDING,             header.EndPadding   );

            Assert.AreEqual( VERSION_STRING,          header.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestSetDefaultsTkGeometry() {
            var header = new MbinHeader();
            header.SetDefaults( typeof( NMS.Toolkit.TkGeometryData ), MbinHeader.Format.V1 );

            Assert.AreEqual( MbinHeader.MBIN_MAGIC_PC, header.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION,  header.FormatID );
            Assert.AreEqual( MbinHeader.MBINCVER_TAG,  header.Tag );
            Assert.AreEqual( VERSION_ID,               header.MbinVersion );
            Assert.AreEqual( "",                       header.TemplateName );
            Assert.AreEqual( END_PADDING,              header.EndPadding );

            Assert.AreEqual( VERSION_STRING,      header.GetMBINVersion().ToString() );
        }

        [TestMethod]
        public void TestSetDefaultsTkAnimMetadata() {
            var header = new MbinHeader();
            header.SetDefaults( typeof( NMS.Toolkit.TkAnimMetadata ), MbinHeader.Format.V1 );

            Assert.AreEqual( MbinHeader.MBIN_MAGIC,             header.MagicID );
            Assert.AreEqual( MbinHeader.MBIN_VERSION,           header.FormatID );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_TAG,     header.Tag );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_VERSION, header.MbinVersion );
            Assert.AreEqual( "",                                header.TemplateName );
            Assert.AreEqual( MbinHeader.TKANIMMETADATA_PADDING, header.EndPadding );

            Assert.AreEqual( NULL_VERSION_STRING,               header.GetMBINVersion().ToString() );
        }

    }

}
