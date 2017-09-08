using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ArkCrossEngine
{
    public class ZipHelper
    {
        public static bool ZipFile(string targetFilePath, string zipFilePath)
        {
            if (!File.Exists(targetFilePath))
            {
                LogicSystem.LogFromGfx("ZipFile failed. targetFilePath not exist: {0}", targetFilePath);
                return false;
            }
            bool needTempZipFile = false;
            if (IsPathEqual(targetFilePath, zipFilePath))
            {
                needTempZipFile = true;
                zipFilePath += ".zip";
            }
            ZipOutputStream zos = null;
            FileStream zipfs = null;
            FileStream fs = null;
            try
            {
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }
                zipfs = File.Create(zipFilePath);
                if (zipfs == null)
                {
                    throw new Exception("zipfs null");
                }
                zos = new ZipOutputStream(zipfs);
                if (zos == null)
                {
                    throw new Exception("zos null");
                }
                zos.SetLevel(9);
                byte[] buffer = new byte[4096];
                ZipEntry entry = new ZipEntry(Path.GetFileName(targetFilePath));
                entry.DateTime = DateTime.Now;
                zos.PutNextEntry(entry);
                fs = File.OpenRead(targetFilePath);
                if (fs == null)
                {
                    throw new Exception("fs null");
                }
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zos.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);

                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (zos != null)
                {
                    zos.Flush();
                    zos.Finish();
                    zos.Close();
                    zos = null;
                }
                if (zipfs != null)
                {
                    zipfs.Close();
                    zipfs = null;
                }

                if (needTempZipFile)
                {
                    File.Delete(targetFilePath);
                    File.Move(zipFilePath, targetFilePath);
                }
            }
            catch (Exception ex)
            {
                LogicSystem.LogFromGfx("ZipFile failed. targetFilePath: {0} zipFilePath:{1} ex:{2}",
                  targetFilePath, zipFilePath, ex);
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                if (zos != null)
                {
                    zos.Finish();
                    zos.Close();
                    zos = null;
                }
                if (zipfs != null)
                {
                    zipfs.Close();
                    zipfs = null;
                }
            }
            return true;
        }
        public static bool UnzipFile(string zipFilePath, string unZipDir)
        {
            if (!File.Exists(zipFilePath))
            {
                LogicSystem.LogFromGfx("UnzipFile failed. zipFilePath not exist: {0}", zipFilePath);
                return false;
            }
            bool needTempZipFile = false;
            string fileName = string.Empty;
            ZipInputStream zis = null;
            FileStream fs = null;
            FileStream streamWriter = null;
            try
            {
                fs = File.OpenRead(zipFilePath);
                if (fs == null)
                {
                    throw new Exception("fs null");
                }
                zis = new ZipInputStream(fs);
                if (fs == null)
                {
                    throw new Exception("zis null");
                }
                ZipEntry theEntry;
                while ((theEntry = zis.GetNextEntry()) != null)
                {
                    //Console.WriteLine(theEntry.Name);
                    fileName = Path.Combine(unZipDir, theEntry.Name);
                    string directoryName = Path.GetDirectoryName(fileName);
                    // create directory
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (IsPathEqual(fileName, zipFilePath))
                    {
                        needTempZipFile = true;
                        fileName += ".bak";
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        streamWriter = File.Create(fileName);
                        if (streamWriter == null)
                        {
                            throw new Exception("streamWriter null");
                        }
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = zis.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (streamWriter != null)
                        {
                            streamWriter.Flush();
                            streamWriter.Close();
                            streamWriter = null;
                        }
                    }
                }
                if (zis != null)
                {
                    zis.Close();
                    zis = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }

                if (needTempZipFile)
                {
                    File.Delete(zipFilePath);
                    File.Move(fileName, zipFilePath);
                }
            }
            catch (System.Exception ex)
            {
                LogicSystem.LogFromGfx("UnzipFile failed. zipFilePath:{0} ex:{1}:{2}", zipFilePath, ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (zis != null)
                {
                    zis.Close();
                    zis = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
            }
            return true;
        }
        public static bool UnzipFile(byte[] buffer, string unZipDir)
        {
            if (buffer == null || buffer.Length <= 0)
            {
                LogicSystem.LogFromGfx("UnzipFile failed. ");
                return false;
            }
            string fileName = string.Empty;
            MemoryStream sm = null;
            ZipInputStream zis = null;
            FileStream streamWriter = null;
            try
            {
                sm = new MemoryStream(buffer);
                if (sm == null)
                {
                    throw new Exception("sm null");
                }
                zis = new ZipInputStream(sm);
                if (zis == null)
                {
                    throw new Exception("zis null");
                }
                ZipEntry theEntry;
                while ((theEntry = zis.GetNextEntry()) != null)
                {
                    //Console.WriteLine(theEntry.Name);
                    fileName = Path.Combine(unZipDir, theEntry.Name);
                    string directoryName = Path.GetDirectoryName(fileName);
                    // create directory
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        streamWriter = File.Create(fileName);
                        if (streamWriter == null)
                        {
                            throw new Exception("streamWriter null");
                        }
                        if (zis.Length > 0)
                        {
                            byte[] data = new byte[zis.Length];
                            int size = zis.Read(data, 0, data.Length);
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                        if (streamWriter != null)
                        {
                            streamWriter.Flush();
                            streamWriter.Close();
                            streamWriter = null;
                        }
                    }
                }
                if (zis != null)
                {
                    zis.Close();
                    zis = null;
                }
                if (sm != null)
                {
                    sm.Close();
                    sm = null;
                }
            }
            catch (System.Exception ex)
            {
                LogicSystem.LogFromGfx("UnzipFile failed. fileName:{0} ex:{1}:{2}", fileName, ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (zis != null)
                {
                    zis.Close();
                    zis = null;
                }
                if (sm != null)
                {
                    sm.Close();
                    sm = null;
                }
            }
            return true;
        }
        private static bool IsPathEqual(string pathA, string pathB)
        {
            pathA = pathA.Replace("\\", "/");
            pathB = pathB.Replace("\\", "/");
            return pathA == pathB;
        }
    }
}
