using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.ArcMapUI;
using Miner.Interop;

namespace ArcFMLoginObject
{
    [Guid("2886ADFA-96B8-43F5-917C-AAE70A0FD38A")]
    [ProgId("ArcFMLoginObject.CacheExt")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class CacheExt:IExtension,ESRI.ArcGIS.esriSystem.IExtensionConfig   
    {
        private string m_sExtensionName = "ArcFMLoginExt";
        private string m_sDescription = "ArcFMLoginExt";
        private string m_sProductName = "ArcFMLoginExt";
        private esriExtensionState m_State = esriExtensionState.esriESEnabled;
        private const string m_sEventSource = "ESRI ArcDesktop Monitor";
        private const string m_sEventLog = "Application";
        private IMxApplication m_pApplication;
        private ITable[] pTables;
        private IFeatureClass[] pFCs;
        private static CacheExt s_extension;

        public CacheExt()
        {
            s_extension = this;
        }

        #region COM GUIDs
        public string ClassId = "39509828-D575-46AE-B23C-DAE235B476B8";
        public string InterfaceId = "804F7449-BD32-4166-BE00-4A30CDF9A28A";
        public string EventsId = "613A9EF4-7F9E-4920-A144-ECC91B21CF61";
        #endregion

        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ESRI.ArcGIS.ADF.CATIDs.MxExtension.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ESRI.ArcGIS.ADF.CATIDs.MxExtension.Unregister(regKey);
        }
        #endregion
        #endregion

        internal static CacheExt GetExtension()
        {
            return s_extension;
        }

        private void InitExt()
        {
            s_extension = this;
        }

        #region IExtensionConfig Members

        public string Description
        {
            get { return m_sDescription; }
        }

        public string ProductName
        {
            get { return m_sProductName; }
        }

        public esriExtensionState State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }

        #endregion

        #region IExtension Members

        public string Name
        {
            get
            {
                return m_sExtensionName;
            }
        }

        public void Shutdown()
        {
            int i;
            try
            {
                for (i = 0; i < pTables.Count(); i++)
                {
                    //Marshal.ReleaseComObject(pTables(i));
                }
            }
            catch (Exception EX)
            {
                if (!EventLog.SourceExists(m_sEventSource))
                    EventLog.CreateEventSource(m_sEventSource, m_sEventLog);
                EventLog.WriteEntry(m_sEventSource, "Error removing counters:" + EX.Message);
            }

            try
            {
            }
            catch (Exception EX)
            {
                if (!EventLog.SourceExists(m_sEventSource))
                    EventLog.CreateEventSource(m_sEventSource, m_sEventLog);
                EventLog.WriteEntry(m_sEventSource, "Error un-wirring Events:" + EX.Message);
            }

            try
            {
                Marshal.ReleaseComObject(m_pApplication);
            }
            catch (Exception EX)
            {
                try
                {
                    if (!EventLog.SourceExists(m_sEventSource))
                        EventLog.CreateEventSource(m_sEventSource, m_sEventLog);
                    EventLog.WriteEntry(m_sEventSource, "Error releasing Memory:" + EX.Message);
                }
                catch
                {
                }
            }
        }

        public void Startup(ref object initializationData)
        {
            IApplication pApp2;
            try
            {

                if ((initializationData is IMxApplication) || (initializationData is IMxApplication2))
                {
                    m_pApplication = (IMxApplication)initializationData;
                    try
                    {

                        try
                        {
                            pApp2 = (IApplication)m_pApplication;

                        }
                        catch (Exception EX)
                        {
                            if (!EventLog.SourceExists(m_sEventSource))
                                EventLog.CreateEventSource(m_sEventSource, m_sEventLog);
                            EventLog.WriteEntry(m_sEventSource, "Error wirring Events:" + EX.Message);
                        }

                    }
                    catch (Exception EX)
                    {
                        try
                        {
                            if (!EventLog.SourceExists(m_sEventSource))
                                EventLog.CreateEventSource(m_sEventSource, m_sEventLog);
                            EventLog.WriteEntry(m_sEventSource, "Error creating Performance Counter Category:" + EX.Message);
                        }
                        catch
                        {
                        }
                    }

                }
            }
            catch (Exception EX)
            {
                try
                {
                    if (!EventLog.SourceExists(m_sEventSource))
                        EventLog.CreateEventSource(m_sEventSource, m_sEventLog);
                    EventLog.WriteEntry(m_sEventSource, "Error creating Performance Counter Category:" + EX.Message);
                }
                catch
                {
                }
            }
        }
        #endregion
        public void OpenClasses(IWorkspace pWKS)
        {
            IObjectClass pOC;
            IFeatureWorkspace pFWKS;
            ICursor pCursor;
            IRow pRow;
            ITable pTable;
            IMMLoginUtils pMMLogin;
            IMMStoredDisplayManager _SDMGR;
            IQueryFilter pQF;
            ITable pTable2;
            int i;
            IWorkspaceFactorySchemaCache pWKSCache;
            IFeatureClass pFC;
            try
            {
                pFWKS = (IFeatureWorkspace)pWKS;
                pWKSCache = (IWorkspaceFactorySchemaCache) pWKS.WorkspaceFactory;
                pWKSCache.EnableAllSchemaCaches();
                pTable = pFWKS.OpenTable("SDE.CACHEOBJECTS");
                if (pTable != null)
                {
                    pQF = new QueryFilter();
                    pQF.SubFields = "OWNER,TABLE_NAME";
                    pQF.WhereClause = "LAYERID = 0";
                    pCursor = pTable.Search(pQF, false);
                    pTables = new ITable[pTable.RowCount(pQF)];
                    pRow = pCursor.NextRow();
                    i=0;
                    while (pRow != null)
                    {
                        try
                        {
                            pTable2 = pFWKS.OpenTable(pRow.get_Value(0) + "." + pRow.get_Value(1));
                            Debug.WriteLine("Table:" + pRow.get_Value(0) + "." + pRow.get_Value(1));
                            pTables[i] = pTable2;
                            i = i + 1;
                        }
                        catch
                        {
                        }
                        pRow = pCursor.NextRow();
                    }
                    pQF.WhereClause = "LAYERID != 0";
                    pCursor = pTable.Search(pQF, false);
                    pFCs = new IFeatureClass[pTable.RowCount(pQF)];
                    pRow = pCursor.NextRow();
                    i = 0;
                    while (pRow != null)
                    {
                        try
                        {
                            pFC = pFWKS.OpenFeatureClass(pRow.get_Value(0) + "." + pRow.get_Value(1));
                            Debug.WriteLine("Feature Class:" + pRow.get_Value(0) + "." + pRow.get_Value(1));
                            pFCs[i] = pFC;
                            i = i + 1;
                        }
                        catch
                        {
                        }
                        pRow = pCursor.NextRow();
                    }
                    Marshal.ReleaseComObject(pCursor);
                }
                pWKSCache.DisableAllSchemaCaches();
            }
            catch (Exception EX)
            {
                Debug.WriteLine("Error:" + EX.Message);
            }
        }
    }
}
